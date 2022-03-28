-- UPDATE OrderFulfillmentLineItems

UPDATE ofli
SET ofli.FulfillmentStartDateTime = oha.auditDate,
    ofli.FulfillmentEndDateTime = oha.auditDate
FROM 
    core.OrderFulfillmentLineItems ofli 
    JOIN core.orderheaders oh ON ofli.orderHeaderId = oh.id
    JOIN core.OrderHeaderAuditLog oha ON oh.id = oha.entityid AND oh.FulfillmentEndDateTime = '0001-01-01 00:00:00.0000000'
WHERE oha.AuditData LIKE '%fulfillmentEndDateTime%' 
    AND oha.auditaction = 'Update'
    AND ofli.FulfillmentStartDateTime = '0001-01-01 00:00:00.0000000'
    AND ofli.FulfillmentEndDateTime = '0001-01-01 00:00:00.0000000'

-- ================================================================================================

-- UPDATE orderheaders

UPDATE oh
SET oh.FulfillmentStartDateTime = oha.auditDate,
    oh.FulfillmentEndDateTime = oha.auditDate
FROM 
    core.orderheaders oh 
    JOIN core.OrderHeaderAuditLog oha ON oh.id = oha.entityid AND oh.FulfillmentEndDateTime = '0001-01-01 00:00:00.0000000'
WHERE oha.AuditData LIKE '%fulfillmentEndDateTime%' 
    AND oha.auditaction = 'Update'