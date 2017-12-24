using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Telegram_Bot.Classes
{
    public struct CryptoCurrency
    {
        private readonly string _currencyName, _marketName, _apiName;

        public CryptoCurrency(string currencyName, string marketName, string apiName)
        {
            this._currencyName = currencyName;
            this._marketName = marketName;
            this._apiName = apiName;
        }

        public string currencyName { get { return _currencyName; } }
        public string marketName { get { return _marketName; } }
        public string apiName { get { return _apiName; } }
    }

    class CurrencyWrapper
    {

        internal List<CryptoCurrency> supportedCurrencies = new List<CryptoCurrency>();

        public CurrencyWrapper()
        {
            SetSupportedCurrencies();
        }

        public string GetCurrencyPrice(string currencyLookup)
        {
            /* Lookup Currency Name */
            CryptoCurrency Currency = supportedCurrencies.Find(x => x.currencyName == currencyLookup);

            if (Currency.currencyName != null)
            {
                switch (Currency.apiName)
                {
                    case "GDAX":
                        return GetGDAXTicker(Currency.marketName);
                    case "CoinMarket":
                        return GetCoinMarketTicker(Currency.marketName);
                    case "Bittrex":
                        return "Not yet implemented.";
                    default:
                        return "Could not find a suitable API.";
                }
            }
            else
            {
                return "Currency not found.";
            }

        }

        internal string GetGDAXTicker(string marketName)
        {
            string TickerUrl = "https://api.gdax.com/products/" + marketName + "/ticker";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TickerUrl);
            request.ContentType = "text/html";
            request.UserAgent = "CryptoNerdBot";
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                var rootResult = JsonConvert.DeserializeObject<GDAXProductTicker>(reader.ReadToEnd());
                return rootResult.price;
            }
        }

        internal string GetCoinMarketTicker(string marketName)
        {
            string TickerUrl = "https://api.coinmarketcap.com/v1/ticker/" + marketName + "/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TickerUrl);
            request.ContentType = "text/html";
            request.UserAgent = "CryptoNerdBot";
            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                var rootResult = JsonConvert.DeserializeObject<List<CoinMarketTicker>>(reader.ReadToEnd());
                return rootResult.First().price_usd;
            }
        }

        internal void SetSupportedCurrencies()
        {
            supportedCurrencies.Add(new CryptoCurrency("ETH", "ETH-USD", "GDAX"));
            supportedCurrencies.Add(new CryptoCurrency("ETH-USD", "ETH-USD", "GDAX"));
            supportedCurrencies.Add(new CryptoCurrency("ETH-BTC", "ETH-BTC", "GDAX"));
            supportedCurrencies.Add(new CryptoCurrency("BTC", "BTC-USD", "GDAX"));
            supportedCurrencies.Add(new CryptoCurrency("BTC-ETH", "ETH-BTC", "GDAX"));
            supportedCurrencies.Add(new CryptoCurrency("BTC-LTC", "LTC-BTC", "GDAX"));
            supportedCurrencies.Add(new CryptoCurrency("LTC", "LTC-USD", "GDAX"));
            supportedCurrencies.Add(new CryptoCurrency("LTC-BTC", "LTC-BTC", "GDAX"));
            supportedCurrencies.Add(new CryptoCurrency("BCH", "bitcoin-cash", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("BCC", "bitcoin-cash", "CoinMarket")); // Referring to BitCoin Cash for now.
            supportedCurrencies.Add(new CryptoCurrency("LBRY", "library-credit", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("LBC", "library-credit", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("SIA", "siacoin", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("SC", "siacoin", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("LUMEN", "stellar", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("STELLAR", "stellar", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("XLM", "stellar", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("XRP", "ripple", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("RIPPLE", "ripple", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("IOTA", "iota", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("MIOTA", "iota", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("DASH", "dash", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("BTG", "bitcoin-gold", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("MONERO", "monero", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("XMR", "monero", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("OMG", "omisego", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("STEEM", "steem", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("DECRED", "decred", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("DCR", "decred", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("GOLEM", "golem-network-tokens", "CoinMarket"));
            supportedCurrencies.Add(new CryptoCurrency("GNT", "golem-network-tokens", "CoinMarket"));
        }
    }
}
