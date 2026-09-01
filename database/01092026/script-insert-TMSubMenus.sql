USE CYDB

INSERT INTO [CYDB].[dbo].[TMSubMenus]
           ([MenuID]
           ,[Seq]
           ,[MenuName_EN]
           ,[MenuName_TH]
           ,[Description]
           ,[CMS_ControllerName]
           ,[CMS_ActionName]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[IsActive])
     VALUES
           (3
           ,45
           ,'New Count Stock'
           ,'นับสต๊อก (ใหม่)'
           ,'หน้ากรอกข้อมูลนับสต๊อกแบบใหม่ สำหรับ PC และหัวหน้า PC'
           ,'Stock'
           ,'NewCountStockEntry'
           ,'admin'
           ,GETDATE()
           ,1)
GO

INSERT INTO [CYDB].[dbo].[TMSubMenus]
           ([MenuID]
           ,[Seq]
           ,[MenuName_EN]
           ,[MenuName_TH]
           ,[Description]
           ,[CMS_ControllerName]
           ,[CMS_ActionName]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[IsActive])
     VALUES
           (3
           ,46
           ,'Stock Comparison'
           ,'เทียบข้อมูลสต๊อก'
           ,'หน้าเปรียบเทียบสต๊อกระบบกับยอดที่นับได้จริง'
           ,'Stock'
           ,'CountStockCompare'
           ,'admin'
           ,GETDATE()
           ,1)
GO

INSERT INTO [CYDB].[dbo].[TMSubMenus]
           ([MenuID]
           ,[Seq]
           ,[MenuName_EN]
           ,[MenuName_TH]
           ,[Description]
           ,[CMS_ControllerName]
           ,[CMS_ActionName]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[IsActive])
     VALUES
           (3
           ,47
           ,'Stock Pending Approval'
           ,'รออนุมัตินับสต๊อก'
           ,'หน้ารายการนับสต๊อกที่รออนุมัติ'
           ,'Stock'
           ,'CountStockPendingApproval'
           ,'admin'
           ,GETDATE()
           ,1)
GO