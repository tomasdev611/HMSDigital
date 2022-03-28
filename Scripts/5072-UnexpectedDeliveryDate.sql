UPDATE ofli
SET ofli.FulfillmentStartDateTime = oh.FulfillmentStartDateTime,
    ofli.FulfillmentEndDateTime = oh.FulfillmentEndDateTime
FROM 
    core.OrderFulfillmentLineItems ofli 
    JOIN core.orderheaders oh ON ofli.orderHeaderId = oh.id
WHERE oh.StatusId = 15 AND
	(ofli.FulfillmentStartDateTime is null or DATEPART(year, ofli.FulfillmentStartDateTime) < 2020)