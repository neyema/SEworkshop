﻿namespace SEWorkshop.Exceptions
{
    [System.Serializable]
    public class BasketIsEmptyException : TradingSystemException
    {
        public BasketIsEmptyException() { }
        public BasketIsEmptyException(string message) : base(message) { }
        public BasketIsEmptyException(string message, System.Exception inner) : base(message, inner) { }
        protected BasketIsEmptyException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        public override string ToString() => "Basket is empty";
    }
}