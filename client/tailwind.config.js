/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}"
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          '50': '#eefcfd',
          '100': '#d4f7f9',
          '200': '#aeedf3',
          '300': '#75dfeb',
          '400': '#3bc9db',
          '500': '#1aabc0',
          '600': '#198aa1',
          '700': '#1b6f83',
          '800': '#1e5b6c',
          '900': '#1e4c5b',
          '950': '#0e313e',
        },
      },
    },
  },
  plugins: [],
}

