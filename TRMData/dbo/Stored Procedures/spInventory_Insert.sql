CREATE PROCEDURE [dbo].[spInventory_Insert]
	@ProductId int,
	@Quantity int,
	@PurchasePrise money,
	@PurchaseDate datetime2
AS
begin
	set nocount on;

	insert into dbo.Inventory(ProductId, Quantity, PurchasePrise, PurchaseDate)
	values(@ProductId, @Quantity, @PurchasePrise, @PurchaseDate)
end