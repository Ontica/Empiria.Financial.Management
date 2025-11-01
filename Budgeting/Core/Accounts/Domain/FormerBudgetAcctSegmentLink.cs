/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Accounts                            Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Link Type                               *
*  Type     : BudgetAccountSegmentLink                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Links a BudgetAccountSegment with other objects, like projects, products or geolocations.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Products;

namespace Empiria.Budgeting {

  /// <summary>Links a BudgetAccountSegment with other objects, like projects,
  /// products or geolocations.</summary>
  public class FormerBudgetAcctSegmentLink : BaseObjectLink {

    #region Types

    static public BaseObjectLinkType ProcurementProductLink =>
                    BaseObjectLinkType.Parse("ObjectTypeInfo.FormerBudgetAccountSegmentLink.ProcurementProduct");

    #endregion Types

    #region Constructors and parsers

    protected FormerBudgetAcctSegmentLink(BaseObjectLinkType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    internal FormerBudgetAcctSegmentLink(FormerBudgetAcctSegment segment, Product product,
                                      string code, string description)
              : base(ProcurementProductLink, segment, product) {

      base.Code = EmpiriaString.Clean(code);
      base.Description = EmpiriaString.Clean(description);
    }

    static public FormerBudgetAcctSegmentLink Parse(int id) => ParseId<FormerBudgetAcctSegmentLink>(id);

    static public FormerBudgetAcctSegmentLink Parse(string uid) => ParseKey<FormerBudgetAcctSegmentLink>(uid);


    static public FixedList<FormerBudgetAcctSegment> GetBudgetAccountSegmentsForProduct(Product product) {
      Assertion.Require(product, nameof(product));

      return GetBaseObjectsFor<FormerBudgetAcctSegment>(ProcurementProductLink, product)
            .Sort((x, y) => x.Code.CompareTo(y.Code));
    }


    static internal FixedList<FormerBudgetAcctSegmentLink> GetListForProduct(Product product) {
      Assertion.Require(product, nameof(product));

      return GetListWithLinkedObject<FormerBudgetAcctSegmentLink>(ProcurementProductLink, product)
            .Sort((x, y) => x.BudgetAccountSegment.Code.CompareTo(y.BudgetAccountSegment.Code));
    }


    #endregion Constructors and parsers

    #region Properties

    internal FormerBudgetAcctSegment BudgetAccountSegment {
      get {
        return base.GetBaseObject<FormerBudgetAcctSegment>();
      }
    }


    public override string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.BudgetAccountSegment.Keywords);
      }
    }

    #endregion Properties

  } // class BudgetAccountSegmentLink

} // namespace Empiria.Budgeting
