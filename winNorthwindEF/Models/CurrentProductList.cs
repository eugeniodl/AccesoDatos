using System;
using System.Collections.Generic;

namespace winNorthwindEF.Models;

public partial class CurrentProductList
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;
}
