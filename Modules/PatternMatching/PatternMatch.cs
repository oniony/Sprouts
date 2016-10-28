using System;

namespace Oniony.Sprouts.PatternMatching
{
    /// <summary>
    /// Adds pattern matching capabilities to .NET.
    /// </summary>
    public static class PatternMatching
    {
        /// <summary>
        /// Starts a pattern match.
        /// </summary>
        /// <param name="obj">The object to pattern match.</param>
        /// <returns>The pattern match context.</returns>
        public static PatternMatch<TTarget, object> PatternMatch<TTarget>(TTarget obj) => new PatternMatch<TTarget, object>(obj);
    }

    /// <summary>
    /// The pattern match context.
    /// </summary>
    /// <typeparam name="TTarget">The target type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    public sealed class PatternMatch<TTarget, TResult>
    {
        #region Public

        public TResult Result => this.matched ? this.result
                                              : default(TResult);

        /// <summary>
        /// Sets the result type of the pattern match.
        /// </summary>
        /// <typeparam name="TNew">The new result type.</typeparam>
        /// <returns>The pattern match for chained matches.</returns>
        public PatternMatch<TTarget, TNew> Returns<TNew>()
        {
            if (this.matched) throw new InvalidOperationException("Cannot change result type once pattern has been matched.");

            return new PatternMatch<TTarget, TNew>(this.target);
        }

        /// <summary>
        /// Applies a value equality pattern match.
        /// </summary>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <typeparam name="TMatch">The type to match.</typeparam>
        /// <param name="match">The value to match.</param>
        /// <param name="action">The action to apply if the match is successful.</param>
        /// <returns>The pattern match context for chained matches.</returns>
        public PatternMatch<TTarget, TResult> Case<TMatch>(TMatch match, Action<TMatch> action)
            where TMatch : TTarget
        {
            if (!this.matched && this.target.Equals(match))
            {
                this.matched = true;
                action((TMatch) this.target);
            }

            return this;
        }

        /// <summary>
        /// Applies a value equality pattern match that produces a result.
        /// </summary>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <typeparam name="TMatch">The type to match.</typeparam>
        /// <param name="match">The value to match.</param>
        /// <param name="function">The function to apply if the match is successful.</param>
        /// <returns>The pattern match context for chained matches.</returns>
        public PatternMatch<TTarget, TResult> Case<TMatch>(TMatch match, Func<TMatch, TResult> function)
            where TMatch : TTarget
        {
            if (!this.matched && this.target.Equals(match))
            {
                this.matched = true;
                this.result = function((TMatch) this.target);
            }

            return this;
        }

        /// <summary>
        /// Applies a value predicate pattern match.
        /// </summary>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <typeparam name="TMatch">The type to match.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="action">The action to apply if the match is successful.</param>
        /// <returns>The pattern match for chained matches.</returns>
        public PatternMatch<TTarget, TResult> Case<TMatch>(Func<TMatch, bool> predicate, Action<TMatch> action)
            where TMatch : TTarget
        {
            if (!this.matched && this.target is TMatch)
            {
                var match = (TMatch) this.target;

                if (predicate(match))
                {
                    this.matched = true;
                    action(match);
                }
            }

            return this;
        }

        /// <summary>
        /// Applies a value predicate pattern match that produces a result.
        /// </summary>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <typeparam name="TMatch">The type to match.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="function">The action to apply if the match is successful.</param>
        /// <returns>The pattern match for chained matches.</returns>
        public PatternMatch<TTarget, TResult> Case<TMatch>(Func<TMatch, bool> predicate, Func<TMatch, TResult> function)
            where TMatch : TTarget
        {
            if (!this.matched && this.target is TMatch)
            {
                var match = (TMatch) this.target;

                if (predicate(match))
                {
                    this.matched = true;
                    this.result = function(match);
                }
            }

            return this;
        }

        /// <summary>
        /// Applies a type pattern match.
        /// </summary>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <typeparam name="TMatch">The type to match.</typeparam>
        /// <param name="action">The action to apply if the match is successful.</param>
        /// <returns>The pattern match for chained matches.</returns>
        public PatternMatch<TTarget, TResult> Case<TMatch>(Action<TMatch> action)
            where TMatch : TTarget
        {
            if (!this.matched && this.target is TMatch)
            {
                this.matched = true;
                action((TMatch) this.target);
            }

            return this;
        }

        /// <summary>
        /// Applies a type pattern match that produces a result.
        /// </summary>
        /// <typeparam name="TTarget">The target type.</typeparam>
        /// <typeparam name="TMatch">The type to match.</typeparam>
        /// <param name="function">The function to apply if the match is successful.</param>
        /// <returns>The pattern match for chained matches.</returns>
        public PatternMatch<TTarget, TResult> Case<TMatch>(Func<TMatch, TResult> function)
            where TMatch : TTarget
        {
            if (!this.matched && this.target is TMatch)
            {
                this.matched = true;
                this.result = function((TMatch) this.target);
            }

            return this;
        }

        /// <summary>
        /// Executes the specified action if no previous matches were successful.
        /// </summary>
        /// <param name="action">The action to run.</param>
        public PatternMatch<TTarget, TResult> Otherwise(Action<TTarget> action)
        {
            if (!this.matched)
            {
                action(this.target);
                this.matched = true;
            }

            return this;
        }

        /// <summary>
        /// Executes the specified action if no previous matches were successful.
        /// </summary>
        /// <param name="function">The function to apply.</param>
        public PatternMatch<TTarget, TResult> Otherwise(Func<TTarget, TResult> function)
        {
            if (!this.matched)
            {
                this.result = function(this.target);
                this.matched = true;
            }

            return this;
        }

        /// <summary>
        /// Throws an exception if matches have been made.
        /// </summary>
        /// <returns></returns>
        public PatternMatch<TTarget, TResult> OtherwiseThrow()
        {
            if (!this.matched) throw new MatchFailureException("The pattern did not match any cases.");

            return this;
        }

        /// <summary>
        /// Implicitly converts the pattern match to the result type.
        /// </summary>
        /// <param name="patternMatch">The pattern match.</param>
        public static implicit operator TResult(PatternMatch<TTarget, TResult> patternMatch) => patternMatch.Result;

        #endregion

        #region Internal

        internal PatternMatch(TTarget target)
        {
            this.target = target;
        }

        #endregion

        #region Private

        private readonly TTarget target;

        private bool matched;
        private TResult result;

        #endregion
    }
}
