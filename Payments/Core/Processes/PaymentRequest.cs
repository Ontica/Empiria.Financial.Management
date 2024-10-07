/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Supplier Payments Procesess                Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Infomation Holder                       *
*  Type     : PaymentRequest                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a supplier payment related request.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.DataObjects;

using Empiria.Workflow.Requests;

namespace Empiria.Payments.Processes {

  /// <summary>Represents a supplier payment related request.</summary>
  public class PaymentRequest : Request {

    protected PaymentRequest(RequestType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    public override FixedList<FieldValue> RequestTypeFields {
      get {
        return new FixedList<FieldValue>();
      }
    }

  }  // class PaymentRequest

}  // namespace Empiria.Payments.Processes
