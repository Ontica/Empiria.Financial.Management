/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Domain objects list                     *
*  Type     : PaymentOrderInstructions                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds and controls the list of payment instructions associated to a payment order.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Payments.Processor.Adapters;

using Empiria.Payments.Data;

namespace Empiria.Payments {

  /// <summary>Holds and controls the list of payment instructions associated to a payment order.</summary>
  public class PaymentOrderInstructions : IEnumerable<PaymentInstruction> {

    private readonly PaymentOrder _paymentOrder;

    private Lazy<List<PaymentInstruction>> _instructions;


    public PaymentOrderInstructions(PaymentOrder paymentOrder) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      _paymentOrder = paymentOrder;

      if (_paymentOrder.IsEmptyInstance) {
        _instructions = new Lazy<List<PaymentInstruction>>(() => new List<PaymentInstruction>());
        return;
      }

      _instructions = new Lazy<List<PaymentInstruction>>(() =>
              PaymentInstructionData.GetPaymentOrderInstructions(_paymentOrder));
    }

    #region Properties

    public int Count {
      get {
        return _instructions.Value.Count;
      }
    }

    public PaymentInstruction Current {
      get {
        if (_instructions.Value.Count > 0) {
          return _instructions.Value[_instructions.Value.Count - 1];
        } else {
          return PaymentInstruction.Empty;
        }
      }
    }

    #endregion Properties

    #region Methods

    public bool CanCreateNewInstruction() {
      if (_paymentOrder.Status == PaymentOrderStatus.Pending ||
          _paymentOrder.Status == PaymentOrderStatus.Failed) {
        return true;
      }
      return false;
    }


    internal PaymentInstruction CreatePaymentInstruction(PaymentsBrokerConfigData broker) {
      Assertion.Require(broker, nameof(broker));

      Assertion.Require(CanCreateNewInstruction(),
               $"No se puede crear la instrucción de pago debido " +
               $"a que tiene el estado {_paymentOrder.Status.GetName()}.");

      var instruction = new PaymentInstruction(broker, _paymentOrder);

      _instructions.Value.Add(instruction);

      _paymentOrder.EventHandler(instruction, PaymentOrderStatus.Programmed);

      return instruction;
    }


    internal void UpdatePaymentInstruction(PaymentInstruction instruction,
                                           BrokerResponseDto brokerResponse) {
      Assertion.Require(instruction, nameof(instruction));
      Assertion.Require(brokerResponse, nameof(brokerResponse));

      instruction.Update(brokerResponse);

      if (brokerResponse.Status == PaymentInstructionStatus.InProgress) {
        _paymentOrder.EventHandler(instruction, PaymentOrderStatus.InProgress);

      } else if (brokerResponse.Status == PaymentInstructionStatus.Failed) {
        _paymentOrder.EventHandler(instruction, PaymentOrderStatus.Failed);
      }
    }

    public IEnumerator<PaymentInstruction> GetEnumerator() {
      return _instructions.Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    #endregion Methods

  }  // class PaymentOrderInstructions

} // namespace Empiria.Payments
