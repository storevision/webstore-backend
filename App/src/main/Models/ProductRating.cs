using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.App.src.main.Models;

[Table("product_ratings")]
public class ProductRating
{
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("one_star")]
    public int OneStar { get; set; }

    [Column("two_stars")]
    public int TwoStars { get; set; }

    [Column("three_stars")]
    public int ThreeStars { get; set; }

    [Column("four_stars")]
    public int FourStars { get; set; }

    [Column("five_stars")]
    public int FiveStars { get; set; }

    [Column("total_reviews")]
    public int TotalReviews { get; set; }

    [Column("average_rating")]
    public decimal AverageRating { get; set; }
}