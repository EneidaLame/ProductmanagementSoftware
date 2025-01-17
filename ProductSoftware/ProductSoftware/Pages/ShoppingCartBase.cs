﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProductSoftware.Services.Contracts;
using ShopOnline.Models.Dtos;

namespace ProductSoftware.Pages
{
    public class ShoppingCartBase:ComponentBase
    {

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }

        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }

       
        

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CartChanged(); 

            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }
        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);
            
            RemoveCartItem(id);
            CartChanged();

        }
        protected async Task UpdateQtyCartitem_Click(int id, int qty)
        {
            if(qty > 0)
            {
                var updateItemDto = new CartItemQtyUpdateDto
                {
                    CartItemId = id,
                    Qty = qty
                };
                var returnedUpdateItemDto = await this.ShoppingCartService.UpdateQty(updateItemDto);
                UpdateItemTotalPrice(returnedUpdateItemDto);
                CartChanged();

            }
            else
            {
                var item = this.ShoppingCartItems.FirstOrDefault(i => i.Id == id);
                if(item == null)
                {
                    item.Qty = 1;
                    item.TotalPrice = item.Price;
                }
            }
        }   
        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
        }
        private void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);
            ShoppingCartItems.Remove(cartItemDto);
        }


        
        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }
        }
        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = this.ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");

        }
        protected void SetTotalQuantity()
        {
            TotalQuantity = this.ShoppingCartItems.Sum(p => p.Qty);
        }
         
        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }

    }
}
