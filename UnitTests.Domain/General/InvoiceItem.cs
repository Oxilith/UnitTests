﻿namespace UnitTests.Domain.General;

public class InvoiceItem
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}