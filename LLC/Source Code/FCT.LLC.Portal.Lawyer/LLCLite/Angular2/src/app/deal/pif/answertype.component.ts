import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { AnswerTypeBase } from './answertype-base';

import { tblAnswer } from "../../core/entities/tblAnswer";

@Component({
    selector: 'answertype',
    templateUrl: 'answertype.html'
})
export class AnswerTypeComponent {
    @Input() answertype: AnswerTypeBase<any>;
    @Input() form: FormGroup;
    get isValid() { return this.form.controls[this.answertype.id].valid; }
    private defaultValidationMessages: { [id: string]: string };

    constructor() {
        this.defaultValidationMessages = {
            'required': '* Mandatory Field',
            'minlength': '* Field must be at least {minlength} characters',
            'maxlength': '* Field cannot exceed {maxlength} characters',
            'pattern': '* Field must be in a valid format'
        }
    }

    isControlValid(controlName: string): boolean {
        var self = this;
        var isValid = true;
        if ((controlName !== null) && (controlName !== undefined) && (controlName.length > 0) && (self.form.controls[controlName] != null)) {
            var controlToValidate = self.form.controls[controlName];
            isValid = (controlToValidate.valid || (controlToValidate.pristine && !controlToValidate.touched));
        }
        
        return isValid;
    }

    getValidationMessage(controlName: string): string {
        var self = this;
        var validationMessage = "";
        if ((controlName !== null) && (controlName !== undefined) && (controlName.length > 0) && (!self.form.valid) && (self.form.controls[controlName] != null)) {
            var controlToValidate = self.form.controls[controlName];
            
            if (controlToValidate.hasError("required")) {
                validationMessage = self.defaultValidationMessages["required"];
            }
            else if (controlToValidate.hasError("minlength")) {
                validationMessage = self.defaultValidationMessages["minlength"];
            }
            else if (controlToValidate.hasError("maxlength")) {
                
                var maxLength = controlToValidate.errors["maxlength"].requiredLength;
                validationMessage = self.defaultValidationMessages["maxlength"];
                validationMessage = validationMessage.replace("{maxlength}", maxLength);
                
            }
            else if (controlToValidate.hasError("pattern")) {
                validationMessage = self.defaultValidationMessages["pattern"];
            }
        }
        
        return validationMessage;
    }

}