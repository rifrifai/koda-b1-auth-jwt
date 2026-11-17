const FormField = ({id, label, type = "text", value, onChange, autoComplete, placeholder, disabled = false}) => {
	return(
		<>
			<div className="space-y-2">
				<label htmlFor={id} className="block text-sm font-bold text-slate-700">
					{label}
				</label>

				<input type={type} id={id} name={id} value={value} onChange={onChange} autoComplete={autoComplete} placeholder={placeholder} disabled={disabled} required className="w-full rounded-xl border border-slate-300 text-slate-900 bg-white px-4 py3 text-sm outline-none transition placeholder:text-slate-400 disabled:cursor-not-allowed disabled:bg-slate-100"/>
			</div>
		</>
	)
};

export default FormField;