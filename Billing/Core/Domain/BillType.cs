/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Power type                              *
*  Type     : BillType                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that represents a bill type like a sales bill type or purchase order bill type,     *
*             a credit note, a paycheck, a payment reception, etc.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;

namespace Empiria.Billing {

  /// <summary>Power type that represents a bill type like a sales bill type or purchase order bill type,
  ///a credit note, a paycheck, a payment reception, etc.</summary>
  [Powertype(typeof(Bill))]
  public sealed class BillType : Powertype {

    #region Constructors and parsers

    private BillType() {
      // Empiria powertype types always have this constructor.
    }

    static public new BillType Parse(int typeId) => Parse<BillType>(typeId);

    static public new BillType Parse(string typeName) => Parse<BillType>(typeName);

    static public BillType Empty => Parse("ObjectTypeInfo.PowerType.BillType");

    #endregion Constructors and parsers

  }  // class BillType

}  // namespace Empiria.Billing
