import { ThemeSwitch } from "./theme-switch";

export default function HeaderVisitant()
{
    return(
        <div className="flex items-center justify-between">
        <div className="flex items-center gap-3">
            <img src="/cloudpins.png" className="h-12 rounded-xl"/>
            <h3 className="text-2xl">CloudPins</h3>
          </div>
          <ThemeSwitch />
        </div>
    );
}