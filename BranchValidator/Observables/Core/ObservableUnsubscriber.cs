// <copyright file="ObservableUnsubscriber.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace BranchValidator.Observables.Core;

    /// <summary>
    /// An <see cref="IObserver{T}"/> unsubscriber for unsubscribing from an <see cref="Observable{TData}"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of data that is pushed to all of the subscribed <see cref="Observer{T}"/>s.
    /// </typeparam>
    public class ObservableUnsubscriber<T> : IDisposable
    {
        private readonly List<IObserver<T>> observers;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableUnsubscriber{T}"/> class.
        /// </summary>
        /// <param name="observers">The list of <see cref="IObserver{T}"/> subscriptions.</param>
        /// <param name="observer">The <see cref="IObserver{T}"/> that has been subscribed.</param>
        internal ObservableUnsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            this.observers = observers ?? throw new ArgumentNullException(nameof(observers), "The parameter must not be null.");
            Observer = observer ?? throw new ArgumentNullException(nameof(observer), "The parameter must not be null.");
        }

        /// <summary>
        /// Gets the <see cref="IObserver{T}"/> of this unsubscription.
        /// </summary>
        public IObserver<T> Observer { get; }

        /// <summary>
        /// Gets the total number of current subscriptions that an <see cref="Observable{TData}"/> has.
        /// </summary>
        public int TotalObservers => this.observers.Count;

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <inheritdoc cref="IDisposable.Dispose"/>
        /// </summary>
        /// <param name="disposing">Disposes managed resources when <c>true</c>.</param>
        private void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.observers.Contains(Observer))
                {
                    this.observers.Remove(Observer);
                }
            }

            this.isDisposed = true;
        }
    }
