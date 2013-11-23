// Define the "namespace"
module MvcApp {

    // Interfaces... in JavaScript!?  WHOA!
    export interface IAmToStringable {
        toString(): string;
    }

    // Class definition (which implements the interface)
    export class MyClass implements IAmToStringable {

        constructor(
                private secret: string,
                public notSecret?: number
            )
        {
        }

        public toString() : string {
            return ['secret: ', this.secret, '; notSecret: ', this.notSecret].join('');
        }
    }
}


// Plain ol' JavaScript
var instance = new MvcApp.MyClass('password', 1234);
instance.notSecret = 7890;
console.log(instance.toString());
