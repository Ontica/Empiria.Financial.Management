/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts milestone item management        Component : Domain Layer                            *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Infomation Holder                       *
*  Type     : ContractMilestoneItem                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract milestone item.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;
using Empiria.Products;
using Empiria.StateEnums;

using Empiria.Budgeting;
using Empiria.Financial;

using Empiria.Contracts.Adapters;
using Empiria.Contracts.Data;


namespace Empiria.Contracts
{

    /// <summary>Represents a contract milestone item.</summary>
    public class ContractMilestoneItem : BaseObject, IPayableEntityItem, INamedEntity
    {

        #region Constructors and parsers

        private ContractMilestoneItem()
        {
            // Required by Empiria Framework.
        }

        public ContractMilestoneItem(ContractMilestoneItemFields fields)
        {
            Assertion.Require(fields, nameof(fields));

            Load(fields);
        }

        static internal ContractMilestoneItem Parse(int id)
        {
            return ParseId<ContractMilestoneItem>(id);
        }

        static internal ContractMilestoneItem Parse(string uid)
        {
            return ParseKey<ContractMilestoneItem>(uid);
        }

        static internal FixedList<ContractMilestoneItem> GetList()
        {
            return GetList<ContractMilestoneItem>().ToFixedList();
        }

        static internal ContractMilestoneItem Empty => ParseEmpty<ContractMilestoneItem>();

        #endregion Constructors and parsers

        #region Properties

        [DataField("MILESTONE_ID")]
        public ContractMilestone ContractMilestone
        {
            get; private set;
        }


        [DataField("CONTRACT_ITEM_ID")]
        public ContractItem ContractItem
        {
            get; private set;
        }


        public string Name
        {
            get
            {
                if (Description.Length != 0)
                {
                    return Description;
                }
                else
                {
                    return Product.Name;
                }
            }
        }


        [DataField("MILESTONE_ITEM_PRODUCT_ID")]
        public Product Product
        {
            get; private set;
        } = Product.Empty;


        [DataField("MILESTONE_ITEM_PRODUCT_UNIT_ID")]
        public ProductUnit ProductUnit
        {
            get; private set;
        } = ProductUnit.Empty;


        [DataField("MILESTONE_ITEM_DESCRIPTION")]
        public string Description
        {
            get; private set;
        } = string.Empty;


        [DataField("MILESTONE_ITEM_QTY", ConvertFrom = typeof(decimal))]
        public decimal Quantity
        {
            get; private set;
        }


        [DataField("MILESTONE_ITEM_UNIT_PRICE", ConvertFrom = typeof(decimal))]
        public decimal UnitPrice
        {
            get; private set;
        }


        public decimal Total
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }


        [DataField("MILESTONE_ITEM_BUDGET_ACCOUNT_ID")]
        public BudgetAccount BudgetAccount
        {
            get;
            private set;
        } = BudgetAccount.Empty;


        [DataField("MILESTONE_ITEM_EXT_DATA")]
        private JsonObject ExtData
        {
            get; set;
        }


        public string Keywords
        {
            get
            {
                return EmpiriaString.BuildKeywords(this.Status.GetName());
            }
        }


        internal Party LastUpdatedBy
        {
            get
            {
                return ExtData.Get("lastUpdatedBy", Party.Empty);
            }
            set
            {
                ExtData.Set("lastUpdatedBy", value.Id);
            }
        }


        internal DateTime LastUpdatedTime
        {
            get
            {
                return ExtData.Get("lastUpdatedTime", this.PostingTime);
            }
            set
            {
                ExtData.Set("lastUpdatedTime", value);
            }
        }


        [DataField("MILESTONE_ITEM_POSTED_BY_ID")]
        public Party PostedBy
        {
            get; private set;
        }


        [DataField("MILESTONE_ITEM_POSTING_TIME")]
        public DateTime PostingTime
        {
            get; private set;
        }


        [DataField("MILESTONE_ITEM_STATUS", Default = EntityStatus.Pending)]
        public EntityStatus Status
        {
            get; private set;
        } = EntityStatus.Pending;


        #endregion Properties


        #region Helpers

        internal void Load(ContractMilestoneItemFields fields)
        {
            this.ContractMilestone = ContractMilestone.Parse(fields.MilestoneUID);
            this.ContractItem = ContractItem.Parse(fields.ContractItemUID);
            this.Description = fields.Description;
            this.Quantity = fields.Quantity;
            this.ProductUnit = ProductUnit.Parse(fields.ProductUnitUID);
            this.Product = Product.Parse(fields.ProductUID);
            this.UnitPrice = fields.UnitPrice;
            this.BudgetAccount = BudgetAccount.Parse(fields.BudgetAccountUID);

        }


        #endregion Helpers


        #region IPayableEntityItem interface

        INamedEntity IPayableEntityItem.BudgetAccount
        {
            get
            {
                return this.BudgetAccount;
            }
        }


        INamedEntity IPayableEntityItem.Currency
        {
            get
            {
                return Currency.Default;
            }
        }


        string IPayableEntityItem.Description
        {
            get
            {
                return this.Name;
            }
        }


        INamedEntity IPayableEntityItem.Product
        {
            get
            {
                return this.Product;
            }
        }


        INamedEntity IPayableEntityItem.Unit
        {
            get
            {
                return this.ProductUnit;
            }
        }


        #endregion IPayableEntityItem interface

    }  // class ContractMilestoneItem

}  // namespace Empiria.Contracts
