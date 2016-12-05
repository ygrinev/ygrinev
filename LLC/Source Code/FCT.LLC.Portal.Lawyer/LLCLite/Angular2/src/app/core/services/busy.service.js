import { Injectable } from '@angular/core';
export var BusyService = (function () {
    function BusyService() {
        this._busyCounter = 0;
    }
    Object.defineProperty(BusyService.prototype, "isBusy", {
        get: function () {
            return this._busyCounter > 0;
        },
        enumerable: true,
        configurable: true
    });
    BusyService.prototype.busy = function (op) {
        var _this = this;
        setTimeout(function () {
            _this._busyCounter++;
            op.then(function (result) {
                _this._busyCounter--;
            }).catch(function (reason) {
                _this._busyCounter--;
            });
        }, 300);
        return op;
    };
    BusyService.decorators = [
        { type: Injectable },
    ];
    /** @nocollapse */
    BusyService.ctorParameters = [];
    return BusyService;
}());
//# sourceMappingURL=busy.service.js.map