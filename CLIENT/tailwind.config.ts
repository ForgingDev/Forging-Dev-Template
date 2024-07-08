import type { Config } from 'tailwindcss';

const config: Config = {
  content: [
    './src/pages/**/*.{js,ts,jsx,tsx,mdx}',
    './src/components/**/*.{js,ts,jsx,tsx,mdx}',
    './src/app/**/*.{js,ts,jsx,tsx,mdx}',
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          light: '#F3F4F6',
          normal: '#1F2937',
          dark: '#111827',
        },
        secondary: {
          light: '#D1D5DB',
          normal: '#6B7280',
          dark: '#374151',
        },
        link: {
          normal: '#3B82F6',
          hover: '#2563EB',
        },
      },
    },
  },
  plugins: [],
};
export default config;
