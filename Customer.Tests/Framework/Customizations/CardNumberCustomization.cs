//using System;
//using Domain.Cards;
//using Ploeh.AutoFixture;

//namespace Domain.Tests.Framework.Customizations
//{
//    public class CardNumberCustomization : ICustomization
//    {
//        public void Customize(IFixture fixture)
//        {
//            fixture.Register(() =>
//            {
//                var cardNumber = new Random().Next(100000000, 999999999);

//                return new CardNumber(cardNumber.ToString());
//            });
//        }
//    }
//}