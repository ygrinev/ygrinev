using System;
using System.ServiceModel;

namespace FCT.EPS.WSP.Resources
{
    public class SafeCommunicationDisposal<T> : IDisposable where T : ICommunicationObject
    {
        public T Instance { get; private set; }
        public SafeCommunicationDisposal(T client)
        {
            this.Instance = client;
        }

        bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Close();
                }
                this.disposed = true;
            }
        }

        private void Close()
        {
            try
            {
                Instance.Close();
            }
            catch (CommunicationException)
            {
                Instance.Abort();
            }
            catch (TimeoutException)
            {
                Instance.Abort();
            }
            catch (Exception)
            {
                Instance.Abort();
                throw;
            }
        }
    }
}
