/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Category Type                           *
*  Type     : BillCategory                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a bill category like product sales or purchase, employee paycheck, etc.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;

namespace Empiria.Billing {

  /// <summary>Represents a bill category like product sales or purchase, employee paycheck, etc.</summary>
  public class BillCategory : DocumentProduct {

    #region Constructors and parsers

    protected BillCategory(DocumentType powerType) : base(powerType) {
      // Required by Empiria Framework
    }

    static public new BillCategory Parse(int id) => ParseId<BillCategory>(id);

    static public new BillCategory Parse(string uid) => ParseKey<BillCategory>(uid);

    static public FixedList<BillCategory> GetList() {
      return BaseObject.GetList<BillCategory>()
                       .ToFixedList();
    }

    static public new BillCategory Empty => ParseEmpty<BillCategory>();

    static public BillCategory FacturaProveedores => Parse(100);

    static public BillCategory NotaDeCreditoPenalizacion => Parse(95);

    static public BillCategory NotaDeCreditoProveedores => Parse(101);

    static public BillCategory ComplementoPagoProveedores => Parse(102);

    static public BillCategory FacturaConsumoCombustible => Parse(93);

    #endregion Constructors and parsers

    #region Properties

    public BillType BillType {
      get {
        return base.Attributes.Get("billTypeId", BillType.Empty);
      }
      private set {
        base.Attributes.SetIf("billTypeId", value.Id, value.Id != -1);
      }
    }


    public bool IsCFDI {
      get {
        return base.Attributes.Get("isCFDI", false);
      }
      private set {
        base.Attributes.SetIf("isCFDI", value, true);
      }
    }


    public string Operation {
      get {
        return base.Attributes.Get("operation", string.Empty);
      }
      private set {
        base.Attributes.SetIf("operation", value, true);
      }
    }


    public override string Keywords {
      get {
        if (IsEmptyInstance) {
          return string.Empty;
        }
        return EmpiriaString.BuildKeywords(base.Keywords, BillType.DisplayName);
      }
    }

    #endregion Properties

  }  // class BillCategory

}  // namespace Empiria.Billing
