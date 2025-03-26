/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Category Type                           *
*  Type     : BillCategory                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a bill category like product sales or purchase, employee paycheck, etc.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Billing {

  /// <summary>Represents a bill category like product sales or purchase, employee paycheck, etc.</summary>
  public class BillCategory : GeneralObject {

    #region Constructors and parsers

    private BillCategory() {
      // Required by Empiria Framework
    }

    internal BillCategory(BillType billType, string name) {
      Assertion.Require(billType, nameof(billType));

      name = EmpiriaString.Clean(name);

      Assertion.Require(name, nameof(name));

      BillType = billType;
      Name = name;
    }

    static public BillCategory Parse(int id) => ParseId<BillCategory>(id);

    static public BillCategory Parse(string uid) => ParseKey<BillCategory>(uid);

    static public FixedList<BillCategory> GetList() {
      return BaseObject.GetList<BillCategory>()
                       .ToFixedList();
    }

    static public BillCategory Empty => ParseEmpty<BillCategory>();

    static public BillCategory FacturaProveedores => Parse(701);

    static public BillCategory NotaDeCreditoProveedores => Parse(702);

    static public BillCategory ComplementoPagoProveedores => Parse(703);

    #endregion Constructors and parsers

    #region Properties

    public BillType BillType {
      get {
        return base.ExtendedDataField.Get("billTypeId", BillType.Empty);
      }
      private set {
        base.ExtendedDataField.SetIf("billTypeId", value.Id, value.Id != -1);
      }
    }


    public string Description {
      get {
        return base.ExtendedDataField.Get("description", string.Empty);
      }
      private set {
        base.ExtendedDataField.SetIfValue("description", EmpiriaString.TrimAll(value));
      }
    }


    public bool IsAssignable {
      get {
        return base.ExtendedDataField.Get("isAssignable", IsEmptyInstance ? false : true);
      }
      private set {
        base.ExtendedDataField.SetIf("isAssignable", value, value == false);
      }
    }


    public BillCategory Parent {
      get {
        return base.ExtendedDataField.Get("parentCategoryId", BillCategory.Empty);
      }
      private set {
        base.ExtendedDataField.SetIf("parentCategoryId", value.Id, value.Id != -1);
      }
    }


    public override string Keywords {
      get {
        if (IsEmptyInstance) {
          return string.Empty;
        }
        return EmpiriaString.BuildKeywords(Name, BillType.DisplayName, Parent.Name);
      }
    }

    #endregion Properties

    #region Methods

    internal void Delete() {
      base.Status = EntityStatus.Deleted;
    }

    #endregion Methods

  }  // class BillCategory

}  // namespace Empiria.Billing
