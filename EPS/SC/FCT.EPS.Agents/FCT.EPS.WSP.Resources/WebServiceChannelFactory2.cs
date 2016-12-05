using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.Generic;

namespace FCT.EPS.WSP.Resources
{
    public class WebServiceChannelFactory2<I> : IDisposable
                               // where B : Binding, new()
    {
        ChannelFactory<I> _channelFactory = null;
        Dictionary<string, I> _channelDictionary = null;
        string _endpointConfigurationName;
        private bool _disposed = false;

        public WebServiceChannelFactory2(string endpointConfigurationName)
        {
            if (string.IsNullOrWhiteSpace(endpointConfigurationName))
            {
                throw new Exception("In valid EndpointConfigurationName ");
            }
            this._endpointConfigurationName = endpointConfigurationName;
            CreateChannelFactory();
            OpenChannelFactory();
        }

        public I GetCachedChannel(string servceEndPoint)
        {
            Init();
            string key = getKey(servceEndPoint);

            I channel = default(I);
            if (_channelDictionary.ContainsKey(key) &&
                _channelDictionary.TryGetValue(key, out channel))
            {
                if (channel != null)
                {
                    try
                    {
                        if (((ICommunicationObject)channel).State == CommunicationState.Faulted)
                        {
                            ((ICommunicationObject)channel).Abort();
                            ((ICommunicationObject)channel).Open();

                        }
                        else if (((ICommunicationObject)channel).State == CommunicationState.Closed)
                        {
                            ((ICommunicationObject)channel).Open();
                        }
                    }
                    catch(System.TimeoutException)
                    {
                        _channelDictionary.Remove(key);
                        channel = _channelFactory.CreateChannel(new EndpointAddress(new Uri(servceEndPoint)));
                        ((ICommunicationObject)channel).Open();
                        _channelDictionary.Add(key, channel);
                    }
                    catch(System.ServiceModel.CommunicationException)
                    {
                        _channelDictionary.Remove(key);
                        channel = _channelFactory.CreateChannel(new EndpointAddress(new Uri(servceEndPoint)));
                        ((ICommunicationObject)channel).Open();
                        _channelDictionary.Add(key, channel);
                    }
                }
            }
            else
            {
                channel = _channelFactory.CreateChannel(new EndpointAddress(new Uri(servceEndPoint)));

                ((ICommunicationObject)channel).Open();
                _channelDictionary.Add(key, channel);
            }

            return channel;
        }

        private static string keyFormat = "{0}_{1}_{2}";
        private string getKey(string servceEndPoint)
        {
            return string.Format(keyFormat, typeof(I).Name, this._endpointConfigurationName, servceEndPoint);
        }

        private void Init()
        {
            if (_channelFactory == null)
            {
                CreateChannelFactory();
                OpenChannelFactory();
            }
            else if (_channelFactory.State == CommunicationState.Faulted)
            {
                ResetChannelFactory();
            }
            if (_channelDictionary == null)
            {
                _channelDictionary = new Dictionary<string, I>();
            }
        }

        private void CreateChannelFactory()
        {
            if (!typeof(I).IsInterface)
            {
//                throw new NotSupportedException(string.Format("The given type {0} is not an interface, thus is not supported", typeof(I).Name));
                throw new Exception(string.Format("The given type {0} is not an interface, thus is not supported", typeof(I).Name));
            }

                _channelFactory = new ChannelFactory<I>(this._endpointConfigurationName);
        }

        private void OpenChannelFactory()
        {
            if (_channelFactory != null)
            {
                _channelFactory.Open();
            }
        }
        private void CloseChannelFactory()
        {
            if (_channelFactory != null)
            {
                CloseChannel();

                if (_channelFactory.State != CommunicationState.Faulted &&
                    _channelFactory.State != CommunicationState.Closed)
                {
                    _channelFactory.Close();
                }
                else
                {
                    _channelFactory.Abort();
                }
            }
        }

        private void CloseChannel()
        {
            I channel = default(I);
            foreach (KeyValuePair<string, I> entry in _channelDictionary)
            {
                channel = entry.Value;

                if (channel != null)
                {
                    if (((ICommunicationObject)channel).State != CommunicationState.Faulted &&
                        ((ICommunicationObject)channel).State != CommunicationState.Closed)
                    {
                        ((ICommunicationObject)channel).Close();
                    }
                    else
                    {
                        ((ICommunicationObject)channel).Abort();
                    }
                }
            }
        }

        private void ResetChannelFactory()
        {
            CloseChannelFactory();
            CreateChannelFactory();
            OpenChannelFactory();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_channelFactory != null)
                    {
                        CloseChannelFactory();
                    }
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}