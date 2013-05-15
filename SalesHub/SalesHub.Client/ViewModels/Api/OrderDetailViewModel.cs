using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SalesHub.Client.ViewModels.Api
{
    public class OrderDetailViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int OrderDetailId { get; set; }

        public string Origin { get; set; }

        [DisplayName("Net Wt")]
        public decimal NetWeight { get; set; }

        [DisplayName("Unit Weight")]
        public decimal UnitWeight { get; set; }

        public int Units { get; set; }

        [DisplayName("Price")]
        public decimal PricePerUnitOfWeight { get; set; }

        [DisplayName("Value Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime ValueDate { get; set; }

        public string Destination { get; set; }

        [DisplayName("Lot number")]
        public string LotNumber { get; set; }

        [DisplayName("Crop year")]
        public int? CropYear { get; set; }

        [HiddenInput(DisplayValue=false)]
        public decimal TotalAmount
        {
            get
            {
                return PricePerUnitOfWeight * Units;
            }
        }

        [HiddenInput(DisplayValue = false)]
        public int OrderId { get; set; }

        public string PackageTypeId { get; set; }
    }
}