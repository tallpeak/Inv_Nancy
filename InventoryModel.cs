namespace Inv_Nancy {
    using System;
    using System.Collections.Generic;
    
    public class InvItem {
        public string Label { get; set; }
        public DateTime Expiration { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
        public InvItem( string label, 
                        DateTime expiration, 
                        string  type, 
                        int count) {
            Label = label;
            Expiration = expiration;
            Type = type;
            Count = count;    
        }
    }
    
    public class Inventory
    {
        private Dictionary<string, InvItem> _inv = new Dictionary<string, InvItem>();
        private Dictionary<string, InvItem> _expired = new Dictionary<string, InvItem>();
        
        // Add an item to inventory, or increment the count if the same label is provided
        // TODO: resolve what to do if the type is different; 
        // eg: should the key of the dictionary change to the composite key of label and type? 
        public InvItem add(InvItem invItem) {
            InvItem ii;
            if(_inv.TryGetValue(invItem.Label, out ii)) {
                ii.Count++;
                return ii;
            } else {
                _inv.Add(invItem.Label, invItem);
                return invItem;
            }
        }             
        // remove ()
        public bool remove(string label)
        {
            InvItem ii;
            if(_inv.TryGetValue(label, out ii)) {
                ii.Count--;
                if (ii.Count >= 1) { 
                    return true; 
                }
                else
                {
                    _inv.Remove(label);
                    return true;
                }
            } else {
                return false;
            }
        }
        
        public Dictionary<string, InvItem> findExpiredItems()
        {
            _expired.Clear();
            foreach (var kv in _inv)
            {
                string key = kv.Key;
                InvItem ii = kv.Value;
                if (ii.Expiration <= DateTime.Now) {
                    _expired.Add(key, ii);
                    _inv.Remove(key);
                }
            }
            return _expired;
        }
    }
}