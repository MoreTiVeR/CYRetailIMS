# Count Stock V2 — สรุปการพัฒนาระบบนับสต๊อกใหม่

**Date:** 2026-09-01

---

## ภาพรวม

ระบบนับสต๊อกใหม่ (V2) ออกแบบมาเพื่อแก้ไขปัญหา UX ของระบบเดิม (V1) ที่ต้องกรอกข้อมูลหลายช่อง และเพิ่ม workflow อนุมัติ 2 ระดับ (PC → HeadPC → Admin)

| | V1 (เดิม) | V2 (ใหม่) |
|---|---|---|
| Input ต่อรายการ | 5 ช่อง (storestock, countedqty, restock, damaged, soldbeforecount) | 1 ช่อง (countedqty) หรือ 6 ช่อง (newentry) |
| หน่วยการนับ | Group by SubItemType | Per Item (ItemID) |
| Workflow | บันทึกตรง | Draft → Submit → Approve |
| ปรับสต๊อก | ทันทีที่บันทึก | เฉพาะเมื่อ Admin อนุมัติ |
| Audit Trail | TTCountStocksHistory | TTCountStocksHistory + TTStockTransaction |

---

## 1. Database / Domain Entities ที่เปลี่ยนแปลง

### TTCountStockDetail — เพิ่ม columns ใหม่

| Column | Type | หมายเหตุ |
|---|---|---|
| `ItemID` | `int?` | รหัสสินค้า รองรับการนับรายสินค้า (V2) |
| `ItemRemark` | `varchar(200)?` | หมายเหตุรายการ (บังคับเมื่อ countedqty = 0) |

### TTCountStock — เพิ่ม columns ใหม่

| Column | Type | หมายเหตุ |
|---|---|---|
| `CountStockStatusID` | `int` | 0=Draft, 1=Submitted, 2=Approved |
| `CounterRole` | `varchar(20)?` | "PC" หรือ "HeadPC" |
| `ApprovedBy` | `varchar(10)?` | ผู้อนุมัติ |
| `ApprovedDate` | `datetime?` | วันที่อนุมัติ |

---

## 2. Application Layer — Command/Query ที่สร้างใหม่

### Commands

| Path | หน้าที่ |
|---|---|
| `CountStockService/Commands/CreateCountStock/v2/` | บันทึกนับสต๊อก V2 — รองรับ Draft/Submit, per-item, partial save, upsert |
| `CountStockService/Commands/SubmitCountStock/v1/` | เปลี่ยนสถานะจาก Draft (0) → Submitted (1) |
| `CountStockService/Commands/ApproveCountStock/v1/` | อนุมัติ (HeadPC เท่านั้น) → ปรับสต๊อกใน TMItemInBranch |

### Queries

| Path | หน้าที่ |
|---|---|
| `CountStockService/Queries/GetPendingApprovals/v1/` | ดึงรายการนับสต๊อกที่รออนุมัติ |
| `CountStockService/Queries/GetCountStockComparison/v1/` | เปรียบเทียบสต๊อกระบบ CY กับยอดนับ (per-item, all statuses) |
| `CountStockService/Queries/InquiryItemsInBranchV2/v1/` | ดึงรายสินค้าของสาขา (per ItemID ไม่ group SubItemType) |

---

## 3. API Layer (ComponentService.API)

| Route | Method | หน้าที่ |
|---|---|---|
| `api/v1/stock/v2/create` | POST | บันทึก/อัปเดต count stock V2 |
| `api/v1/stock/v1/submit` | POST | Submit draft |
| `api/v1/stock/v1/approve` | POST | อนุมัติ + ปรับสต๊อก |
| `api/v1/stock/v1/pending-approvals` | POST | รายการรออนุมัติ |
| `api/v1/stock/v1/comparison` | POST | ตารางเปรียบเทียบ |
| `api/v1/stock/v1/inquiry-items-bybranch-v2` | POST | รายสินค้า per item |

---

## 4. Infrastructure Layer

ไฟล์: `Infrastructure/ExternalService/CountStockAPI/CountStockAPI.cs`

| Method ใหม่ | เรียก Route |
|---|---|
| `CreateCountStockListV2Async` | `api/v1/stock/v2/create` |
| `SubmitCountStockAsync` | `api/v1/stock/v1/submit` |
| `ApproveCountStockAsync` | `api/v1/stock/v1/approve` |
| `GetPendingApprovalsAsync` | `api/v1/stock/v1/pending-approvals` |
| `GetCountStockComparisonAsync` | `api/v1/stock/v1/comparison` |
| `InquiryItemsInBranchV2Async` | `api/v1/stock/v1/inquiry-items-bybranch-v2` |

Interface: `Application/ExternalService/CountStockAPI/ICountStockAPI.cs` — เพิ่ม method ทั้งหมดข้างต้น

---

## 5. Web Layer (Controller)

ไฟล์: `ComponentService.Web/Controllers/StockController.cs`

### Actions ใหม่ (GET)

| Action | Route | หน้าที่ |
|---|---|---|
| `NewCountStockEntry` | `Stock/NewCountStockEntry` | หน้านับสต๊อก (PC / HeadPC) |
| `CountStockCompare` | `Stock/CountStockCompare` | หน้าเทียบข้อมูลสต๊อก |
| `CountStockPendingApproval` | `Stock/CountStockPendingApproval` | หน้ารออนุมัติ |
| `ExportCountStockExcel` | `Stock/ExportCountStockExcel?countstockid=` | ดาวน์โหลด Excel รายการนับสต๊อก |

### Actions ใหม่ (POST/API)

| Action | หน้าที่ |
|---|---|
| `GetItemStockDataByBranch` | โหลดสต๊อกรายสินค้า พร้อมเติม draft ที่บันทึกไว้ |
| `GetCountStockComparison` | ตารางเปรียบเทียบ CY stock vs นับได้ |
| `GetPendingApprovals` | รายการรออนุมัติ |
| `SaveDraftCountStock` | บันทึก draft (status=0) |
| `SubmitNewCountStock` | ส่งข้อมูล (status=1) |
| `SubmitCountStockNew` | เปลี่ยนสถานะ draft → submitted |
| `ApproveCountStockNew` | อนุมัติ + ปรับสต๊อก (Admin เท่านั้น) |

### Private Methods ใหม่

| Method | หน้าที่ |
|---|---|
| `PrepareSelectSubItemType` | โหลด dropdown จาก `TMSubItemType` จริง (แทน TMItemType) |
| `PrepareNewCountStockCommand` | แปลง `NewCountStockEntryModel` → `CreateCountStockCommandV2` |
| `ParseDate` | แปลงวันที่หลายรูปแบบ (dd/MM/yyyy, yyyy-MM-dd, ฯลฯ) |
| `ExportCountStockExcel` | สร้างไฟล์ Excel ด้วย EPPlus.Core |

---

## 6. Views ที่สร้างใหม่

| View | หน้าที่ |
|---|---|
| `Views/Stock/NewCountStockEntry.cshtml` | หน้ากรอกนับสต๊อก — PC/HeadPC ใช้ |
| `Views/Stock/CountStockCompare.cshtml` | หน้าเทียบสต๊อก CY vs ยอดนับ พร้อม filter วันที่ |
| `Views/Stock/CountStockPendingApproval.cshtml` | หน้ารออนุมัติ — Admin กด Approve ได้เฉพาะ HeadPC |

---

## 7. JavaScript ที่สร้างใหม่

| File | หน้าที่ |
|---|---|
| `wwwroot/js/view/countstock/countstock_newentry.js` | โหลดสต๊อก, กรอกนับ, save draft, submit, filter, export Excel |
| `wwwroot/js/view/countstock/countstock_compare.js` | โหลดตาราง comparison, filter, export |
| `wwwroot/js/view/countstock/countstock_approval.js` | โหลดรายการรออนุมัติ, กด approve, download Excel per row |

---

## 8. Models ที่สร้างใหม่

| File | หน้าที่ |
|---|---|
| `Application/Common/Models/UI/NewCountStockEntryModel.cs` | Model รับข้อมูลจาก JS สำหรับหน้า NewCountStockEntry |
| `Application/Common/Models/UI/SearchCountStockComparisonViewModel.cs` | Search params หน้า compare |
| `Application/Common/Models/UI/SearchPendingApprovalViewModel.cs` | Search params หน้า approval |
| `Application/Common/Models/UI/SubmitCountStockViewModel.cs` | Request body submit |
| `Application/Common/Models/UI/ApproveCountStockViewModel.cs` | Request body approve |

---

## 9. Bug Fixes ที่แก้ไขระหว่างพัฒนา

### GetCountStockComparisonHandler

| Bug | สาเหตุ | การแก้ |
|---|---|---|
| `pc_countedqty` แสดง 0 | Status filter excludes Draft (0) | รวม status ทั้งหมด |
| `pc_countedqty` ผิด | ใช้ `CountedAmountQty` ไม่ใช่ `TotalCountQty` | เปลี่ยนเป็น `TotalCountQty` |
| แสดงน้อยกว่า 499 rows | Group by SubItemType แทน per-item | เปลี่ยนเป็น per-item (`itemsInBranch.ToList()` ไม่ GroupBy) |
| lookup ผิด | ใช้ SubItemTypeID key | เพิ่ม per-ItemID key (V2) + SubItemTypeID fallback (V1 legacy) |

### DropDown ประเภทย่อย (CountStockCompare & NewCountStockEntry)

| Bug | สาเหตุ | การแก้ |
|---|---|---|
| Filter ไม่เจอข้อมูล | ใช้ `TMItemType` (Case/Film/Equipment) แทน `TMSubItemType` | เปลี่ยนเป็น `PrepareSelectSubItemType()` → `TMSubItemType.SubItemCode` |
| JS filter ผิด | `applyClientFilter` กรองจาก `itemtypecode` | เปลี่ยนกรองจาก `subitemcode` |

### ApproveCountStockHandler

| Bug | สาเหตุ | การแก้ |
|---|---|---|
| NullReferenceException | `QueryAsync` ไม่ eager-load `Item` navigation | เปลี่ยนเป็น `FindWithInclude` + `Include(s => s.Item)` |
| Double-approve race condition | Status check นอก transaction | Re-check status ภายใน transaction |
| ไม่มี audit trail | ไม่สร้าง TTStockTransaction | เพิ่ม TTStockTransaction ทุก item ที่ Qty เปลี่ยน |
| In-transit stock หาย | ไม่นำ pending transfer กลับมาบวก | Load TTItemTransfer Pending → บวก inTransit qty |
| Rounding loss V1 | `Math.Round` → total ≠ target | `Math.Floor` + last item absorbs remainder |

### ExportCountStockExcel

| Bug | สาเหตุ | การแก้ |
|---|---|---|
| 404 Not Found | Action ไม่มีใน Controller | เพิ่ม `ExportCountStockExcel(int countstockid)` GET action ด้วย EPPlus.Core |

---

## 10. Workflow สรุป

```
PC (Sale role)
  └─ NewCountStockEntry → SaveDraft (status=0) → SubmitNewCountStock (status=1)

HeadPC (SaleArea role)
  └─ NewCountStockEntry → SaveDraft (status=0) → SubmitNewCountStock (status=1)

Admin
  └─ CountStockPendingApproval → ApproveCountStockNew
       ├─ ตรวจสอบ status=1 (ภายใน transaction)
       ├─ ตรวจสอบ CounterRole=HeadPC
       ├─ ปรับ TMItemInBranch.Qty (บวก in-transit กลับ)
       ├─ สร้าง TTStockTransaction (audit)
       └─ เปลี่ยนสถานะเป็น Approved (2)

Admin / Stock / SaleArea / Sale
  └─ CountStockCompare → เปรียบเทียบ CY stock vs ยอดนับ per-item
```

---

## 11. การ Authorize แต่ละหน้า

| หน้า | Role ที่เข้าได้ |
|---|---|
| NewCountStockEntry | Sale, SaleArea |
| CountStockCompare | Admin, Stock, SaleArea, Sale |
| CountStockPendingApproval | Admin, SaleArea |
| ApproveCountStockNew (POST) | Admin เท่านั้น |
| ExportCountStockExcel | Admin, SaleArea, Stock, Sale |
