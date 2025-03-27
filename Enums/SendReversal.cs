namespace moneygram_api.Enums
{
    /// <summary>
    /// Represents the reason for reversing a send transaction.
    /// </summary>
    public enum SendReversalReason
    {
        /// <summary>
        /// No receive location available.
        /// </summary>
        NO_RCV_LOC,

        /// <summary>
        /// Wrong delivery option selected.
        /// </summary>
        WRONG_SERVICE,

        /// <summary>
        /// Missing test question and answer.
        /// </summary>
        NO_TQ,

        /// <summary>
        /// Incorrect send or receive amount.
        /// </summary>
        INCORRECT_AMT,

        /// <summary>
        /// MoneyGram Plus or a card number not used.
        /// </summary>
        MS_NOT_USED
    }

    /// <summary>
    /// Represents the type of reversal for a send transaction.
    /// </summary>
    public enum ReversalType
    {
        /// <summary>
        /// Cancel: Reversal done on the same day as the transaction (US Central Time).
        /// </summary>
        C,

        /// <summary>
        /// Refund: Reversal done the day after the send transaction (US Central Time).
        /// Agent application must restrict this for same-day refunds.
        /// </summary>
        R
    }

    public enum RefundFee
    {
        /// <summary>
        /// No: Fee not refunded.
        /// </summary>
        N,

        /// <summary>
        /// Yes: Fee refunded.
        /// </summary>
        Y
    }
}