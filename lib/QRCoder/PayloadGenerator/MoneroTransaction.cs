using System;

namespace QRCoder;

public static partial class PayloadGenerator
{
    /// <summary>
    /// Generates a Monero transaction payload for QR codes.
    /// </summary>
    public class MoneroTransaction : Payload
    {
        private readonly string _address;
        private readonly string? _txPaymentId, _recipientName, _txDescription;
        private readonly float? _txAmount;

        /// <summary>
        /// Creates a Monero transaction payload.
        /// </summary>
        /// <param name="address">Receiver's Monero address.</param>
        /// <param name="txAmount">Amount to transfer.</param>
        /// <param name="txPaymentId">Payment ID.</param>
        /// <param name="recipientName">Recipient's name.</param>
        /// <param name="txDescription">Reference text / payment description.</param>
        /// <exception cref="MoneroTransactionException">Thrown when the address is null or empty, or when the txAmount is less than or equal to 0.</exception>
        public MoneroTransaction(string address, float? txAmount = null, string? txPaymentId = null, string? recipientName = null, string? txDescription = null)
        {
            if (string.IsNullOrEmpty(address))
                throw new MoneroTransactionException("The address is mandatory and has to be set.");
            _address = address;
            if (txAmount != null && txAmount <= 0)
                throw new MoneroTransactionException("Value of 'txAmount' must be greater than 0.");
            _txAmount = txAmount;
            _txPaymentId = txPaymentId;
            _recipientName = recipientName;
            _txDescription = txDescription;
        }

        /// <summary>
        /// Returns the Monero transaction payload as a URI string.
        /// </summary>
        /// <returns>The Monero transaction payload as a URI string.</returns>
        public override string ToString()
        {
            var moneroUri = $"monero://{_address}{(!string.IsNullOrEmpty(_txPaymentId) || !string.IsNullOrEmpty(_recipientName) || !string.IsNullOrEmpty(_txDescription) || _txAmount != null ? "?" : string.Empty)}";
            moneroUri += (!string.IsNullOrEmpty(_txPaymentId) ? $"tx_payment_id={Uri.EscapeDataString(_txPaymentId)}&" : string.Empty);
            moneroUri += (!string.IsNullOrEmpty(_recipientName) ? $"recipient_name={Uri.EscapeDataString(_recipientName)}&" : string.Empty);
            moneroUri += (_txAmount != null ? $"tx_amount={_txAmount.ToString()!.Replace(",", ".")}&" : string.Empty);
            moneroUri += (!string.IsNullOrEmpty(_txDescription) ? $"tx_description={Uri.EscapeDataString(_txDescription)}" : string.Empty);
            return moneroUri.TrimEnd('&');
        }


        /// <summary>
        /// Exception class for Monero transaction errors.
        /// </summary>
        public class MoneroTransactionException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MoneroTransactionException"/> class.
            /// </summary>
            public MoneroTransactionException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="MoneroTransactionException"/> class with a specified error message.
            /// </summary>
            /// <param name="message">The message that describes the error.</param>
            public MoneroTransactionException(string message)
                : base(message)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="MoneroTransactionException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
            /// </summary>
            /// <param name="message">The message that describes the error.</param>
            /// <param name="inner">The exception that is the cause of the current exception.</param>
            public MoneroTransactionException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }
    }
}
