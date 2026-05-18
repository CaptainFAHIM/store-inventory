using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public int Qty { get; set; }

    public int Cid { get; set; }

    public virtual Category CidNavigation { get; set; } = null!;
}
