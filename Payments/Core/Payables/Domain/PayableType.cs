/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Power type                              *
*  Type     : PayableType                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes payable types partitioning payable objects.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Ontology;
using Empiria.Reflection;

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

    #region Properties

    internal ObjectTypeInfo PayableEntityType {
      get {
        int payableEntityTypeId = ExtensionData.Get<int>("payableEntityTypeId");

        return ObjectTypeInfo.Parse(payableEntityTypeId);
      }
    }

    #endregion Properties

    #region Methods

    internal IPayableEntity ParsePayableEntity(string payableEntityUID) {

      Type type = PayableEntityType.UnderlyingSystemType;

      object instance = ObjectFactory.InvokeParseMethod(type, payableEntityUID);

      return (IPayableEntity) instance;
    }


    internal IPayableEntity ParsePayableEntity(int payableEntityId) {

      Type type = PayableEntityType.UnderlyingSystemType;

      object instance = ObjectFactory.InvokeParseMethod(type, payableEntityId);

      return (IPayableEntity) instance;
    }

    #endregion Methods

  }  // class PayableType

}  // namespace Empiria.Payments.Payables
