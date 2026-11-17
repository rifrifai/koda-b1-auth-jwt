import { useState } from "react";
import FormField from "../Components/Forms/FormField";
import { loginUser } from "../services/authApi";

const LoginPage = () => {
	const [formData, setFormData] = useState({
		email: "",
		password: "",
	});

	const [isLoading, setIsLoading] = useState(false);

	const [errorMessage, setErrorMessage] = useState("");

	const [successMessage, setSuccessMessage] = useState("");

	const [verifiedUser, setVerifiedUser] = useState(null);

	const handleChange = (event) => {
		const { name, value } = event.target;

		setFormData((previous) => ({
			...previous,
			[name]: value,
		}));

		setErrorMessage("");
		setSuccessMessage("");

		setVerifiedUser(null);
	}
	
	const handleSubmit = async (event) => {
		event.preventDefault();

		if (isLoading) return;

		setErrorMessage("");
		setSuccessMessage("");
		setVerifiedUser(null);

		if (!formData.email.trim() || !formData.password) {
			setErrorMessage("Email dan password wajib diisi");
			return;
		}

		setIsLoading(true);

		try {
			const response = await loginUser(formData);

			setVerifiedUser(response.user);

			setSuccessMessage("Email dan password cocok.");

			setFormData((previous) => ({
				...previous,
				password: "",
			}));
		} catch(error) {
			setErrorMessage(
				error instanceof Error
				? error.message
				: "Terjadi kesalahan saat login.",
			);
			} finally {
			setIsLoading(false);
			}
		}
	
	return(
		<main className="min-h-screen items-center bg-slate-950 px-4 py-10 text-slate-900">
			<div className="mx-auto items-center grid min-h-[85vh] max-w-5xl overflow-hidden rounded-3xl bg-white shadow-2xl md:grid-cols-2">
				<section className="flex items-center p-7 sm:p-12">
				<div className="w-full">
					<p className="text-sm font-bold tracking-widest text-orange-600">
					KODASTORE
					</p>

					<h2 className="mt-3 text-3xl font-bold text-slate-900">Sign in</h2>

					<p className="mt-3 text-sm text-slate-500">
					Masukkan email dan password akun KodaStore.
					</p>

					<form onSubmit={handleSubmit} className="mt-8 space-y-5">
					<FormField
						id="email"
						label="Email"
						type="email"
						value={formData.email}
						onChange={handleChange}
						autoComplete="email"
						placeholder="email@example.com"
					/>

					<FormField
						id="password"
						label="Password"
						type="password"
						value={formData.password}
						onChange={handleChange}
						autoComplete="current-password"
						placeholder="Masukkan password"
					/>

					<button
						type="submit"
						disabled={
						isLoading || !formData.email.trim() || !formData.password
						}
						className="w-full rounded-xl bg-orange-600 px-5 py-3 font-semibold text-white transition hover:bg-orange-700 disabled:cursor-not-allowed disabled:opacity-60"
					>
						{isLoading ? "Memverifikasi..." : "Login"}
					</button>
					</form>

					{errorMessage && (
					<div
						role="alert"
						className="mt-5 rounded-xl border border-red-200 bg-red-50 p-4 text-sm text-red-700"
					>
						{errorMessage}
					</div>
					)}

					{successMessage && (
					<div
						role="status"
						className="mt-5 rounded-xl border border-green-200 bg-green-50 p-4 text-sm text-green-800"
					>
						<p className="font-semibold">Verifikasi berhasil</p>

						<p className="mt-1">{successMessage}</p>

						{verifiedUser && (
						<p className="mt-2">User: {verifiedUser.displayName}</p>
						)}
					</div>
					)}
				</div>
				</section>
			</div>
		</main>
	);
}

export default LoginPage;