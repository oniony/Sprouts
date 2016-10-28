using System;

namespace Oniony.Sprouts.PatternMatching
{
    public sealed class MatchFailureException : ApplicationException
    {
        #region Public

        public MatchFailureException(string message) : base(message)
        {
            // nowt taken out
        }

        #endregion
    }
}
