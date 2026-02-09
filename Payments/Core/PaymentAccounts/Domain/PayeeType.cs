/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Payments.Core.dll                    Pattern   : Value Type                            *
*  Type     : PayeeType                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds a static list of payee types.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

namespace Empiria.Payments {

  /// <summary>Holds a static list of payee types.</summary>
  public class PayeeType : NamedEntity {

    private static readonly FixedList<PayeeType> _payeeTypes = null;

    #region Constructors and parsers

    static PayeeType() {
      _payeeTypes = ReadPayeeTypes();
    }

    private PayeeType(string name) : base(name, name) {

    }

    static public PayeeType Parse(string uid) {
      PayeeType supplierType = _payeeTypes.Find(x => x.UID == uid);

      if (supplierType == null) {
        return Unknown;
      }

      return supplierType;
    }

    static public FixedList<PayeeType> GetPayeeTypes() {
      return _payeeTypes;
    }

    static public PayeeType Unknown => Parse("Desconocido");

    #endregion Constructors and parsers

    static private FixedList<PayeeType> ReadPayeeTypes() {

      var storedJson = StoredJson.Parse("SupplierTypes");

      var types = storedJson.Value.GetFixedList<string>("types");

      return types.Select(x => new PayeeType(x))
                  .ToFixedList();
    }

  }  // class PayeeType

}  // namespace Empiria.Payments
