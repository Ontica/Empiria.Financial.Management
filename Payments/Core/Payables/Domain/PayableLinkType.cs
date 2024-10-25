/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information Holder                      *
*  Type     : PayableLinkType                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  :  Describes links between payable object with other base object                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Financial;
using Empiria.Ontology;
using Empiria.Reflection;

namespace Empiria.Payments.Payables {

  /// <summary> Describes links between payable object with other base object. </summary>
  internal class PayableLinkType : GeneralObject {

    #region Constructors and parsers

    static public PayableLinkType Parse(int id) => ParseId<PayableLinkType>(id);

    static public PayableLinkType Parse(string uid) => ParseKey<PayableLinkType>(uid);

    static public FixedList<PayableLinkType> GetList() {
      return BaseObject.GetList<PayableLinkType>()
                       .ToFixedList();
    }

    static public PaymentMethod Empty => ParseEmpty<PaymentMethod>();


    #endregion Constructors and parsers


    #region Properties

    public ObjectTypeInfo LinkedObjectType {
      get {
        int id  = ExtendedDataField.Get<int>("linkedObjectTypeId");

        return ObjectTypeInfo.Parse(id);
      }
    }

    #endregion Properties

    #region Methods

    public BaseObject ParseLinkedObject(string linkedObjectUID) {
      Assertion.Require(linkedObjectUID, nameof(linkedObjectUID));

      Type type = LinkedObjectType.UnderlyingSystemType;

      object instance = ObjectFactory.InvokeParseMethod(type, linkedObjectUID);

      return (BaseObject) instance;
    }

    internal BaseObject ParsePayableEntity(int payableEntityId) {

      Type type = LinkedObjectType.UnderlyingSystemType;

      object instance = ObjectFactory.InvokeParseMethod(type, payableEntityId);

      return (BaseObject) instance;
    }

    #endregion Methods

  }  // class PayableLinkType

}  // namespace Empiria.Payments.Payables