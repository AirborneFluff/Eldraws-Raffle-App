import { BehaviorSubject, Observable } from 'rxjs';

export function Observe<T>(observedKey: string): PropertyDecorator {
  // `target` defines the target class prototype that the property decorator
  // is attached to.
  return (target: any, key: string | symbol): void => {
    // Declare all the active subjects for a given target class instance.
    const subjects = new WeakMap<any, BehaviorSubject<T | undefined>>();

    // Return the associated subject for a given target class instance.
    // In case none is available yet, create one.
    const getSubject = (
      instance: any
    ): BehaviorSubject<T | undefined> | undefined => {
      if (!subjects.has(instance)) {
        subjects.set(instance, new BehaviorSubject<T | undefined>(undefined));
      }
      return subjects.get(instance);
    };

    // Transform the decorated property into an `Observable` that propagates
    // the changes of the internal subject.
    Object.defineProperty(target, key, {
      get(): Observable<T | undefined> | undefined {
        // `this` is the current instance of the class
        return getSubject(this);
      },
    });

    // Transform the definition of the observed property so that we can propagate
    // its value changes to the internal subject.
    Object.defineProperty(target, observedKey, {
      get(): T | undefined {
        return getSubject(this)?.getValue();
      },
      set(instanceNewValue: T): void {
        getSubject(this)?.next(instanceNewValue);
      },
    });
  };
}
