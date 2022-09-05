using ETicaretAPI.Application.ViewModels.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandRequest :IRequest<CreateProductCommandResponse>
    {
      

        //public VM_Create_Product Model { get; set; }
            public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
    }
}
