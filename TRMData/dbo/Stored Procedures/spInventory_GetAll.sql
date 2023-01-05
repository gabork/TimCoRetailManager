CREATE PROCEDURE [dbo].[spInventory_GetAll]
AS
begin
	set nocount on;

	select [ProductId], [Quantity], [PurchasePrise], [PurchaseDate]
	from dbo.Inventory;
end