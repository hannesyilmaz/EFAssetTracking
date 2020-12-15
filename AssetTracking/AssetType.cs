using System;
using System.Collections.Generic;
using System.Text;

namespace AssetTracking
{
    public class AssetType
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public AssetType(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
        public AssetType(string name)
        {
            this.Name = name;
        }
    }
}
