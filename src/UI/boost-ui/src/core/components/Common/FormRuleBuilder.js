export class FormRuleBuilder {
    constructor(field, form) {
        this.field = field;
        this.form = form;
        this.rules = [];
    }

    addRequired(min, condition) {
        this.rules.push((v) => this._isMatch(condition, v) && !!v || `${this.field} is required`)
        if (min) {
            this.rules.push((v) => this._isMatch(condition, v) && (v && v.length >= min) || `${this.field} must be at least ${min} characters`)
        }
        return this;
    }
    _isMatch(condition, v) {
        if (!condition) {
            return true;
        }
        else {
            return condition(this.form, v);
        }
    }

    addRule(rule) {
        this.rules.push(rule);
    }

    build() {
        return this.rules;
    }
}