/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service provider                        *
*  Type     : PaymentsTimeControl                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to control payments payment instructions timing periods and rules.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Time;

namespace Empiria.Payments {

  /// <summary>Provides services to control payment instructions timing periods and rules.</summary>
  internal class PaymentsTimeControl {

    private readonly PaymentInstruction _instruction;

    private PaymentsTimeWindow _timeWindow;

    static private TimeSpan WAITING_REQUEST_SECONDS = TimeSpan.FromSeconds(60);
    static private TimeSpan WAITING_PAYMENT_MINUTES = TimeSpan.FromMinutes(20);

    internal PaymentsTimeControl(PaymentInstruction instruction) {
      Assertion.Require(instruction, nameof(instruction));

      _instruction = instruction;
      _timeWindow = PaymentsTimeWindow.Instance;
    }


    public void EnsureCanRequestPayment() {

      if (DateTime.Today.IsNonWorkingDate()) {
        Assertion.RequireFail("No es posible enviar pagos en días no laborables.");
      }

      if (_timeWindow.IsInDefaultTimeWindow(DateTime.Now)) {
        return;
      }

      if (_instruction.IsUrgent && _timeWindow.IsInUrgentTimeWindow(DateTime.Now)) {
        return;

      } else if (!_instruction.IsUrgent && _timeWindow.IsInUrgentTimeWindow(DateTime.Now)) {

        Assertion.RequireFail("En este momento solo es posible enviar pagos con prioridad urgente.");

      } else if (_instruction.IsUrgent && !_timeWindow.IsInUrgentTimeWindow(DateTime.Now)) {

        Assertion.RequireFail("La instrucción de pago es urgente. " +
                              "Sin embargo, es necesario solicitar una ampliación del horario de envío.");

      } else if (!_instruction.IsUrgent && !_timeWindow.IsInUrgentTimeWindow(DateTime.Now)) {

        Assertion.RequireFail("No es posible enviar pagos con prioridad normal en este horario.");

      } else {
        throw Assertion.EnsureNoReachThisCode("Unhandled code condition.");

      }
    }


    public bool PaymentRequestTimeElapsed {
      get {
        var paymentRequestEntry = _instruction.LogEntries
                                              .FindLast(x => x.Status == PaymentInstructionStatus.PaymentConfirmation);

        if (paymentRequestEntry == null) {
          return false;
        }

        return paymentRequestEntry.TimeStamp.Add(WAITING_PAYMENT_MINUTES) <= DateTime.Now;
      }
    }


    public bool WaitingRequestTimeElapsed {
      get {
        var waitingRequestEntry = _instruction.LogEntries
                                              .FindLast(x => x.Status == PaymentInstructionStatus.WaitingRequest);

        if (waitingRequestEntry == null) {
          return false;
        }

        return waitingRequestEntry.TimeStamp.Add(WAITING_REQUEST_SECONDS) <= DateTime.Now;
      }
    }

  }  // class PaymentsTimeControl

}  // namespace Empiria.Payments
