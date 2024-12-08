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
using Empiria.Projects;

namespace Empiria.Budgeting {

  /// <summary>Links a BudgetAccountSegment with other objects, like projects,
  /// products or geolocations.</summary>
  public class BudgetAccountSegmentLink : BaseObjectLink {

    #region Types

    static public BaseObjectLinkType ProductLink =>
                    BaseObjectLinkType.Parse("ObjectTypeInfo.BudgetAccountSegmentLink.Product");

    #endregion Types

    #region Constructors and parsers

    protected BudgetAccountSegmentLink(BaseObjectLinkType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    internal BudgetAccountSegmentLink(BudgetAccountSegment segment, Product product)
              : base(ProductLink, segment, product) {
      // no-op
    }

    static public BudgetAccountSegmentLink Parse(int id) => ParseId<BudgetAccountSegmentLink>(id);

    static public BudgetAccountSegmentLink Parse(string uid) => ParseKey<BudgetAccountSegmentLink>(uid);


    static public FixedList<BudgetAccountSegment> GetBudgetAccountSegmentsForProduct(Product product) {
      Assertion.Require(product, nameof(product));

      return GetBaseObjectsFor<BudgetAccountSegment>(ProductLink, product)
            .Sort((x, y) => x.Code.CompareTo(y.Code));
    }


    static internal FixedList<BudgetAccountSegmentLink> GetListForProduct(Product product) {
      Assertion.Require(product, nameof(product));

      return GetListWithLinkedObject<BudgetAccountSegmentLink>(ProductLink, product)
            .Sort((x, y) => x.BudgetAccountSegment.Code.CompareTo(y.BudgetAccountSegment.Code));
    }


    #endregion Constructors and parsers

    #region Properties

    internal BudgetAccountSegment BudgetAccountSegment {
      get {
        return base.GetBaseObject<BudgetAccountSegment>();
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
