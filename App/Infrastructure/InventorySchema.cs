using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure
{
    public static class InventorySchema
    {
        public static void EnsureWorkflowTables(PmsCSp26Context db)
        {
            db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[PurchaseOrders]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PurchaseOrders](
        [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_PurchaseOrders] PRIMARY KEY,
        [OrderNumber] NVARCHAR(30) NOT NULL,
        [SupplierName] NVARCHAR(100) NOT NULL,
        [Status] NVARCHAR(20) NOT NULL,
        [Notes] NVARCHAR(500) NULL,
        [CreatedAt] DATETIME2 NOT NULL,
        [ApprovedAt] DATETIME2 NULL,
        [ReceivedAt] DATETIME2 NULL,
        [CancelledAt] DATETIME2 NULL
    );
END");

            db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[PurchaseOrderItems]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PurchaseOrderItems](
        [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_PurchaseOrderItems] PRIMARY KEY,
        [PurchaseOrderId] INT NOT NULL,
        [ProductId] INT NULL,
        [ProductName] NVARCHAR(50) NOT NULL,
        [Qty] INT NOT NULL,
        [UnitCost] FLOAT NOT NULL,
        CONSTRAINT [FK_PurchaseOrderItems_PurchaseOrders] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [dbo].[PurchaseOrders]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PurchaseOrderItems_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products]([Id]) ON DELETE SET NULL
    );
END");

            db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[StockMovements]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[StockMovements](
        [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_StockMovements] PRIMARY KEY,
        [ProductId] INT NULL,
        [ProductName] NVARCHAR(50) NOT NULL,
        [PurchaseOrderId] INT NULL,
        [PurchaseOrderNumber] NVARCHAR(30) NULL,
        [MovementType] NVARCHAR(50) NOT NULL,
        [QuantityChange] INT NOT NULL,
        [PreviousQty] INT NOT NULL,
        [NewQty] INT NOT NULL,
        [Notes] NVARCHAR(200) NULL,
        [CreatedAt] DATETIME2 NOT NULL,
        CONSTRAINT [FK_StockMovements_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products]([Id]) ON DELETE SET NULL,
        CONSTRAINT [FK_StockMovements_PurchaseOrders] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [dbo].[PurchaseOrders]([Id]) ON DELETE SET NULL
    );
END");
        }
    }
}