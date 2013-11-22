module MvcApp {

    export class MyClass {

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

var instance = new MvcApp.MyClass('password', 1234);
instance.notSecret = 7890;
console.log(instance.toString());
