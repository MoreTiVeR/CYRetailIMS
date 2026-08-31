USE CYDB


--ADMIN - เทียบข้อมูลสต๊อก(Stock Comparison)
INSERT INTO TMRoleInMenus (RoleID, MenuID, SubMenuID, CanView, CanCreate, CanEdit, CanDelete, CreatedBy, CreatedDate, IsActive)
VALUES (1, 49, 3, 1, 1, 1, 1, 'admin', GETDATE(), 1)
GO

--ADMIN - รออนุมัตินับสต๊อก(Stock Pending Approval)
INSERT INTO TMRoleInMenus (RoleID, MenuID, SubMenuID, CanView, CanCreate, CanEdit, CanDelete, CreatedBy, CreatedDate, IsActive)
VALUES (1, 50, 3, 1, 1, 1, 1, 'admin', GETDATE(), 1)

--SALE, PC - รออนุมัตินับสต๊อก(Stock Pending Approval)
INSERT INTO TMRoleInMenus (RoleID, MenuID, SubMenuID, CanView, CanCreate, CanEdit, CanDelete, CreatedBy, CreatedDate, IsActive)
VALUES (2, 48, 3, 1, 1, 1, 1, 'admin', GETDATE(), 1)

--Sale Area, HeaderPC - รออนุมัตินับสต๊อก(Stock Pending Approval)
INSERT INTO TMRoleInMenus (RoleID, MenuID, SubMenuID, CanView, CanCreate, CanEdit, CanDelete, CreatedBy, CreatedDate, IsActive)
VALUES (5, 48, 3, 1, 1, 1, 1, 'admin', GETDATE(), 1)