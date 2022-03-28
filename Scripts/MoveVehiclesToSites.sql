ALTER TABLE core.OrderFulfillmentLineItems  
NOCHECK CONSTRAINT FK_OrderLineItemFulfilment_Vehicles;  
GO  
--OrderFulfillmentLineItems data and Constraint update
update o set FulfilledByVehicleId = s.Id from core.OrderFulfillmentLineItems o 
inner join core.Vehicles v on o.FulfilledByVehicleId = v.Id
left join core.Sites s on s.CVN = v.CVN
where o.FulfilledByVehicleId is not null;
GO
ALTER TABLE [core].[OrderFulfillmentLineItems] DROP CONSTRAINT [FK_OrderLineItemFulfilment_Vehicles];
GO
ALTER TABLE [core].[OrderFulfillmentLineItems] WITH NOCHECK
    ADD CONSTRAINT [FK_OrderLineItemFulfilment_Vehicles] FOREIGN KEY ([FulfilledByVehicleId]) REFERENCES [core].[Sites] ([Id]);
GO
ALTER TABLE [core].[OrderFulfillmentLineItems] WITH CHECK CHECK CONSTRAINT [FK_OrderLineItemFulfilment_Vehicles];
GO
print 'OrderFulfillmentLineItems update'
ALTER TABLE core.OrderFulfillmentLineItems  
CHECK CONSTRAINT FK_OrderLineItemFulfilment_Vehicles;  
GO 

-- update inventories with current location Ids as existing truck
update i set CurrentLocationTypeId = 1, CurrentLocationId = s.Id from core.Inventory i 
inner join core.Vehicles v on i.CurrentLocationId = v.Id
left join core.Sites s on s.CVN = v.CVN
where i.CurrentLocationId is not null and i.CurrentLocationTypeId = 2;

