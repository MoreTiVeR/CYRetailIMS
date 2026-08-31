# ApproveCountStockHandler — Analysis & Fixes

**Date:** 2026-09-01  
**File:** `src/Application/Services/CountStockService/Commands/ApproveCountStock/v1/ApproveCountStockHandler.cs`

---

## ปัญหาที่พบและการแก้ไข

### 🔴 Critical 1 — Race Condition: Double-Approve

**ปัญหา**  
Status check (`CountStockStatusID != 1`) เกิดขึ้น **ก่อน** `BeginTransactionAsync`  
ถ้า Admin 2 คนกด Approve พร้อมกัน → ทั้งคู่เห็น status=1 → ทั้งคู่ผ่าน check → สต๊อกถูกปรับ 2 รอบ

```csharp
// โค้ดเดิม (ผิด)
if (countStock.CountStockStatusID != 1) throw ...   // ← ก่อน transaction
await _unitOfWork.BeginTransactionAsync();
```

**การแก้ไข**  
Re-check status ด้วย fresh query **ภายใน** transaction หลัง `BeginTransactionAsync`

```csharp
// โค้ดใหม่ (ถูกต้อง)
await _unitOfWork.BeginTransactionAsync();

var freshStatus = (await _unitOfWork.Repository<TTCountStock>()
    .QueryAsync(w => w.CountStockID == request.countstockid))
    .Select(s => s.CountStockStatusID)
    .FirstOrDefault();

if (freshStatus != 1)
    throw new Exception("ไม่สามารถอนุมัติได้ เนื่องจากสถานะรายการไม่ถูกต้อง (อาจถูกอนุมัติไปแล้ว)");
```

---

### 🔴 Critical 2 — ไม่มี Audit Trail

**ปัญหา**  
ปรับ `TMItemInBranch.Qty` โดยตรง โดยไม่สร้าง `TTStockTransaction`  
→ ไม่มีบันทึกว่าสต๊อกเปลี่ยนแปลงเพราะอะไร  
→ รายงานสต๊อก, transaction log พัง

**การแก้ไข**  
สร้าง `TTStockTransaction` ทุกครั้งที่ Qty เปลี่ยน

```csharp
int delta = adjustedTarget - item.Qty;
if (delta != 0)
{
    stockAdjustments.Add(new TTStockTransaction
    {
        StockTypeID     = delta > 0 ? 1 : 2,  // 1=In, 2=Out
        ItemID          = item.ItemID,
        Qty             = Math.Abs(delta),
        TransactionDate = approvedAt,
        CreatedBy       = request.approvedby,
        CreatedDate     = approvedAt,
        IsActive        = true
    });
}
```

---

### 🟡 Important 3 — In-Transit Stock ไม่ถูกนำมาคิด

**ปัญหา**  
ถ้ามีสินค้าค้างโอน (`TTItemTransfer`, TransferStatus=Pending, SourceID = branch นี้)  
Qty ถูกตัดออกจาก source branch แล้ว แต่ HeadPC อาจนับรวมสินค้าเหล่านั้น  
→ approve → in-transit qty หายออกจาก system

**การแก้ไข**  
โหลด pending transfers ของ branch → บวก in-transit qty กลับก่อนเขียน Qty

```csharp
var inTransitQtyByItem = pendingTransfersQuery
    .GroupBy(g => g.ItemID)
    .ToDictionary(k => k.Key, v => v.Sum(s => s.Qty));

int inTransit = inTransitQtyByItem.TryGetValue(item.ItemID, out var t) ? t : 0;
int adjustedTarget = targetQty + inTransit;
item.Qty = adjustedTarget;
```

---

### 🟡 Important 4 — Rounding Loss ใน V1 Path

**ปัญหา**  
`Math.Round(proportion)` บน N items → total อาจ ≠ target  
ตัวอย่าง: 3 items แต่ละชิ้น Qty=5, target=10 → each rounds to 3 → total=9 ≠ 10 (สูญหาย 1 ชิ้น)

```csharp
// โค้ดเดิม (ผิด)
item.Qty = (int)Math.Round((double)item.Qty / totalCurrentQty * targetQty);
```

**การแก้ไข**  
ใช้ `Math.Floor` + ให้ item สุดท้ายรับ remainder ที่เหลือ → total ตรงเสมอ

```csharp
// โค้ดใหม่ (ถูกต้อง)
int distributed = 0;
for (int idx = 0; idx < itemsInSubType.Count; idx++)
{
    int share = totalCurrentQty > 0
        ? (int)Math.Floor((double)item.Qty / totalCurrentQty * targetQty)
        : targetQty / itemsInSubType.Count;

    if (idx == itemsInSubType.Count - 1)
        share = (targetQty - distributed) + inTransit;  // last item absorbs remainder
    else
        share += inTransit;

    distributed += (share - inTransit);
    item.Qty = Math.Max(0, share);
}
```

---

## สรุปผลกระทบ

| ประเด็น | ความรุนแรง | สถานะ |
|---|---|---|
| Race condition double-approve | 🔴 Critical | ✅ แก้แล้ว |
| ไม่มี audit trail TTStockTransaction | 🔴 Critical | ✅ แก้แล้ว |
| In-transit stock ไม่ถูกนำมาคิด | 🟡 Important | ✅ แก้แล้ว |
| Rounding loss ใน V1 path | 🟡 Important | ✅ แก้แล้ว |
| Sales ระหว่างนับถึงอนุมัติ | ⚪ Accepted | — limitation ปกติของ periodic count |
| Items ที่ไม่อยู่ใน detail (partial save) | ⚪ Accepted | — Qty คงเดิม, เหมาะกับ partial count |
| PO ที่ค้างรับ | ⚪ N/A | — PO รับจาก Warehouse ไม่กระทบ Branch |
