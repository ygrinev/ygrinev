using FCT.EPS.WSP.ExternalResources.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.Resources
{
    public class TokenizerSerializer
    {
        public static string DeTokenize(string passedTokenValue)
        {
            using (SafeCommunicationDisposal<TokenizationClient> myTokenizationClient = new SafeCommunicationDisposal<TokenizationClient>(new TokenizationClient()))
            {
                return myTokenizationClient.Instance.Untokenize(passedTokenValue);
            }

        }

        public static string Tokenize(string passedValueToTokenize)
        {
            using (SafeCommunicationDisposal<TokenizationClient> myTokenizationClient = new SafeCommunicationDisposal<TokenizationClient>(new TokenizationClient()))
            {
                return myTokenizationClient.Instance.Tokenize(passedValueToTokenize);
            }
        }
    }
}
