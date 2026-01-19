import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { PAGES } from '../config/page';
import { Loader } from 'lucide-react';

export default function GuestOnlyRoute() {
    const { isAuthenticated, isLoading } = useAuth();

    if (isLoading) {
        return (
            <div className="flex justify-center items-center min-h-screen">
                <Loader className="w-8 h-8 text-brand-600 animate-spin" />
            </div>
        );
    }

    if (isAuthenticated) {
        return <Navigate to={PAGES.HOME.PATH} replace />;
    }

    return <Outlet />;
}
