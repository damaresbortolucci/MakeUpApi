using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace MakeupAPI.Dto
{
    public class ProductDto
    {
 
        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("price")]
        public string? Price { get; set; }

        [JsonPropertyName("image_link")]
        public string? Image_Link { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("rating")]
        public float Rating { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("product_type")]
        public string? Product_Type { get; set; }


        public ProductDto(string brand, string name, string price, string image_Link, string description, float rating, string category, string product_Type)
        {
            Brand = brand;
            Name = name;
            Price = price;
            Image_Link = image_Link;
            Description = description;
            Rating = rating;
            Category = category;
            Product_Type = product_Type;
        }
    }
}