/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Power type                              *
*  Type     : PayableType                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes payable types partitioning payable objects.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;

namespace Empiria.Payments.Payables {

  /// <summary>Power type that describes payable types partitioning payable objects.</summary>
  [Powertype(typeof(Payable))]
  internal class PayableType : Powertype {

    #region Constructors and parsers

    private PayableType() {
      // Empiria powertype types always have this constructor.
    }

    static public new PayableType Parse(int typeId) {
      if (typeId != -1) {
        return Parse<PayableType>(typeId);
      } else {
        return Empty;
      }
    }

    static public new PayableType Parse(string typeName) {
      return Parse<PayableType>(typeName);
    }

    static public FixedList<PayableType> GetList() {
      return BasePayableType.ExtensionData.GetFixedList<PayableType>("payableTypes");
    }

    static public PayableType Empty {
      get {
        return Parse("ObjectType.Payable.Empty");
      }
    }

    static private ObjectTypeInfo BasePayableType => Powertype.Parse("ObjectTypeInfo.PowerType.PayableType");

    static public PayableType ContractMilestone => Parse("ObjectTypeInfo.Payable.ContractMilestone");

    #endregion Constructors and parsers

  }  // class PayableType

}  // namespace Empiria.Payments.Payables
